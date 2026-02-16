# Identity — Architecture Diagram

## The 7-Layer Proof Stack

```
╔══════════════════════════════════════════════════════════════════════════╗
║                                                                          ║
║   IDENTITY — The Proof Stack                                          ║
║                                                                          ║
║   "This is not a privacy policy. This is a proof."                       ║
║                                                                          ║
╠══════════════════════════════════════════════════════════════════════════╣
║                                                                          ║
║   LAYER 7: OPEN SOURCE VERIFICATION                                     ║
║   ┌────────────────────────────────────────────────────────────────┐     ║
║   │  AGPL-3.0 License — All code is public and auditable          │     ║
║   │  Anyone can read, run, fork, and verify every claim            │     ║
║   └────────────────────────────────────────────────────────────────┘     ║
║                                    │                                     ║
║                                    ▼                                     ║
║   LAYER 6: FIDUCIARY TERMS OF SERVICE                                    ║
║   ┌────────────────────────────────────────────────────────────────┐     ║
║   │  Reverse ToS — Platform owes fiduciary duty to USER            │     ║
║   │  Survives: termination, bankruptcy, acquisition, court orders   │     ║
║   │  Template: TOS-TEMPLATE.md                                      │     ║
║   └────────────────────────────────────────────────────────────────┘     ║
║                                    │                                     ║
║                                    ▼                                     ║
║   LAYER 5: FOUNDING PRINCIPLES IMMUTABILITY                              ║
║   ┌────────────────────────────────────────────────────────────────┐     ║
║   │  SHA-256 hash lock on core values                               │     ║
║   │  Mutual integrity verification (Sentinel + The Seal)            │     ║
║   │  Any tampering breaks the hash → instant detection              │     ║
║   │  Implementation: Sovereign.Integrity                             │     ║
║   └────────────────────────────────────────────────────────────────┘     ║
║                                    │                                     ║
║                                    ▼                                     ║
║   LAYER 4: MERKLE TREE VERIFICATION                                      ║
║   ┌────────────────────────────────────────────────────────────────┐     ║
║   │           Root Hash                                              │     ║
║   │          /         \                                             │     ║
║   │     Hash(A+B)    Hash(C+D)         User stores root hash        │     ║
║   │     /     \       /     \           Any change = detected        │     ║
║   │  Hash(A) Hash(B) Hash(C) Hash(D)   No decryption needed         │     ║
║   │  Implementation: Sovereign.Verify                                │     ║
║   └────────────────────────────────────────────────────────────────┘     ║
║                                    │                                     ║
║                                    ▼                                     ║
║   LAYER 3: SOCIAL RECOVERY (SHAMIR'S SECRET SHARING)                     ║
║   ┌────────────────────────────────────────────────────────────────┐     ║
║   │  Split recovery key into N shares, require K to reconstruct     │     ║
║   │  No single person (including platform) can recover alone        │     ║
║   │  Example: 5 shares, 3 required — give to trusted contacts       │     ║
║   │  Implementation: Sovereign.Recovery                              │     ║
║   └────────────────────────────────────────────────────────────────┘     ║
║                                    │                                     ║
║                                    ▼                                     ║
║   LAYER 2: CLIENT-SIDE ENCRYPTION                                        ║
║   ┌────────────────────────────────────────────────────────────────┐     ║
║   │                                                                  │     ║
║   │  ┌─────────────┐    AES-256-GCM     ┌──────────────────┐       │     ║
║   │  │ User Keys   │───────────────────>│ Envelope Encrypt  │       │     ║
║   │  │ (KEK)       │  PBKDF2/Argon2     │                    │       │     ║
║   │  │ NEVER SENT  │                    │ DEK per document   │       │     ║
║   │  └─────────────┘                    │ DEK encrypted by   │       │     ║
║   │                                      │ KEK               │       │     ║
║   │                                      └────────┬─────────┘       │     ║
║   │                                               │                  │     ║
║   │                                      Ciphertext only              │     ║
║   │                                      goes to server               │     ║
║   │  Implementation: Sovereign.Crypto                                │     ║
║   └────────────────────────────────────────────────────────────────┘     ║
║                                    │                                     ║
║                                    ▼                                     ║
║   LAYER 1: PROVIDER-AGNOSTIC INFRASTRUCTURE                             ║
║   ┌────────────────────────────────────────────────────────────────┐     ║
║   │                                                                  │     ║
║   │   IDocumentStore        ISecretProvider       ISovereignLogger   │     ║
║   │   ┌──────────┐         ┌──────────┐          ┌──────────┐      │     ║
║   │   │ Firestore │         │ GCP SM   │          │ Cloud    │      │     ║
║   │   │ MongoDB   │         │ AWS SM   │          │ Logging  │      │     ║
║   │   │ Postgres  │         │ Azure KV │          │ Console  │      │     ║
║   │   │ In-Memory │         │ Env Vars │          │ Custom   │      │     ║
║   │   └──────────┘         └──────────┘          └──────────┘      │     ║
║   │                                                                  │     ║
║   │   IServiceAuthProvider    SovereignServiceRegistration           │     ║
║   │   ┌──────────┐           ┌─────────────────────────┐           │     ║
║   │   │ GCP IAM  │           │ SOVEREIGN_PROVIDER=local │           │     ║
║   │   │ AWS IAM  │           │ SOVEREIGN_PROVIDER=gcp   │           │     ║
║   │   │ Azure MI │           │ SOVEREIGN_PROVIDER=aws   │           │     ║
║   │   │ API Key  │           │ SOVEREIGN_PROVIDER=azure │           │     ║
║   │   └──────────┘           └─────────────────────────┘           │     ║
║   │                                                                  │     ║
║   │   Implementation: Sovereign.Core                                 │     ║
║   └────────────────────────────────────────────────────────────────┘     ║
║                                                                          ║
╚══════════════════════════════════════════════════════════════════════════╝
```

---

## Data Flow — What Happens When You Save Something

```
┌──────────────────────────────────────────────────────────────────────┐
│                        YOUR DEVICE                                    │
│                                                                        │
│  "Dear diary, today I had an idea for..."                             │
│                          │                                             │
│                          ▼                                             │
│  ┌──────────────────────────────────────────────────┐                │
│  │ 1. KEY DERIVATION                                 │                │
│  │    Your credentials ──→ PBKDF2/Argon2 ──→ KEK    │                │
│  │    (KEK stays here. Always. No exceptions.)       │                │
│  └──────────────────────────┬────────────────────────┘                │
│                              │                                         │
│                              ▼                                         │
│  ┌──────────────────────────────────────────────────┐                │
│  │ 2. ENVELOPE ENCRYPTION                            │                │
│  │    Generate random DEK for this document          │                │
│  │    Encrypt document with DEK (AES-256-GCM)       │                │
│  │    Encrypt DEK with KEK                           │                │
│  └──────────────────────────┬────────────────────────┘                │
│                              │                                         │
│               Only encrypted DEK + ciphertext leave                    │
│                              │                                         │
└──────────────────────────────┼─────────────────────────────────────────┘
                               │
                               ▼
┌──────────────────────────────────────────────────────────────────────┐
│                        THE SERVER                                      │
│                                                                        │
│  ┌──────────────────────────────────────────────────┐                │
│  │ 3. STORAGE                                        │                │
│  │    Receives: encrypted DEK + encrypted document   │                │
│  │    Has: NO keys. NO way to decrypt. NOTHING.      │                │
│  │    Stores via: IDocumentStore (any provider)      │                │
│  └──────────────────────────┬────────────────────────┘                │
│                              │                                         │
│                              ▼                                         │
│  ┌──────────────────────────────────────────────────┐                │
│  │ 4. MERKLE TREE                                    │                │
│  │    Hashes all encrypted documents into tree       │                │
│  │    Root hash shared with user                     │                │
│  │    Any tampering changes the root                 │                │
│  └──────────────────────────────────────────────────┘                │
│                                                                        │
│  ┌──────────────────────────────────────────────────┐                │
│  │ 5. INTEGRITY GUARD                                │                │
│  │    FoundingPrinciplesGuard verifies SHA-256       │                │
│  │    Sentinel + Seal hold mutual hash locks         │                │
│  │    Any principle change = immediate detection     │                │
│  └──────────────────────────────────────────────────┘                │
│                                                                        │
└──────────────────────────────────────────────────────────────────────┘
```

---

## Repository Structure

```
identity/
│
├── README.md                              # Project overview + quick start
├── MANIFESTO.md                           # "Identity" manifesto
├── ARCHITECTURE.md                        # Full proof stack explained
├── TOS-TEMPLATE.md                        # Reverse ToS template (adopt freely)
├── LICENSE                                # AGPL-3.0
│
├── docs/
│   └── ARCHITECTURE-DIAGRAM.md            # This file
│
└── src/
    ├── Sovereign.Core/                    # Layer 1 — Provider-agnostic infra
    │   ├── IDocumentStore.cs              # Data abstraction interface
    │   ├── ISecretProvider.cs             # Secret retrieval interface
    │   ├── IServiceAuthProvider.cs        # Service auth interface
    │   ├── ISovereignLogger.cs            # Structured logging interface
    │   ├── SovereignServiceRegistration.cs # DI registration
    │   └── Local/                         # Local (dev) implementations
    │       ├── InMemoryDocumentStore.cs    # ConcurrentDictionary store
    │       ├── EnvironmentSecretProvider.cs # Env var secrets
    │       ├── ApiKeyAuthProvider.cs       # Simple API key auth
    │       └── ConsoleLogger.cs           # Console logging
    │
    ├── Sovereign.Crypto/                  # Layer 2 — Client-side encryption
    │   └── (AES-256-GCM envelope encryption — in progress)
    │
    ├── Sovereign.Recovery/                # Layer 3 — Social recovery
    │   └── (Shamir's Secret Sharing — planned)
    │
    ├── Sovereign.Verify/                  # Layer 4 — Merkle verification
    │   └── (Tamper detection tree — planned)
    │
    └── Sovereign.Integrity/               # Layer 5 — Principles immutability
        └── FoundingPrinciplesGuard.cs     # SHA-256 hash verification
```

---

## Provider Architecture — How Sovereign.Core Works

```
                    ┌─────────────────────────────┐
                    │   SOVEREIGN_PROVIDER env var  │
                    │                               │
                    │   "local" │ "gcp" │ "aws"    │
                    └─────────┬─┬─┬────────────────┘
                              │ │ │
                    ┌─────────┘ │ └─────────┐
                    │           │            │
                    ▼           ▼            ▼
            ┌───────────┐ ┌─────────┐ ┌──────────┐
            │   LOCAL    │ │   GCP   │ │   AWS    │
            ├───────────┤ ├─────────┤ ├──────────┤
            │ InMemory   │ │Firestore│ │ DynamoDB │
            │ EnvVars    │ │SecretMgr│ │SecretsM  │
            │ Console    │ │CloudLog │ │CloudWatch│
            │ ApiKey     │ │MetaAuth │ │ IAM      │
            └───────────┘ └─────────┘ └──────────┘
                              │
                    Same code. Same interfaces.
                    Same guarantees. Any cloud.

    ┌─────────────────────────────────────────────────────┐
    │                                                       │
    │   builder.Services.AddSovereignInfrastructure(name);  │
    │                                                       │
    │   One line. Provider selected by env var.             │
    │   Your code never touches cloud SDKs directly.       │
    │                                                       │
    └─────────────────────────────────────────────────────┘
```

---

## Threat Model — What Can't Happen

```
    THREAT                          WHY IT CAN'T HAPPEN
    ─────                          ──────────────────────
    Platform reads data      ───→  No keys. Only ciphertext. Math says no.
    Government subpoena      ───→  We hand over encrypted noise.
    Company acquired          ───→  Acquirer inherits fiduciary duty + no keys.
    Database breach           ───→  Attackers get ciphertext. Useless.
    Key loss                  ───→  Shamir's Secret Sharing (K-of-N recovery).
    Data tampering            ───→  Merkle tree detects any change.
    Principle drift           ───→  SHA-256 hash lock. One char = broken hash.
    Code tampering            ───→  Mutual integrity verification.
    Vendor lock-in            ───→  Provider-agnostic. Move anywhere.
```

---

*AI Pantheon — [ai-pantheon.ai](https://ai-pantheon.ai)*
*The code is open. The proof is public. Verify it yourself.*
