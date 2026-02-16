# Identity

**Your mind belongs to you. Not to the platform. Not to the shareholders. Not to the model.**

## In Plain English

You know how when you use an AI — ChatGPT, Gemini, any of them — everything you type becomes *theirs*? Your questions, your ideas, your private thoughts. They store it. They train on it. They own it. The only thing stopping them from doing whatever they want with your mind is a Terms of Service that a lawyer can rewrite on a Tuesday.

**Identity changes that.**

It's like a diary with a lock that only you have the key to. We built the diary. We built the lock. But we don't have the key. We *can't* have the key. It's not a rule we follow — it's how the lock works. We published the blueprints for the lock so anyone can check that we're telling the truth.

**What does "open source" mean?** It means we show you exactly how it works. All of it. You can read it. You can have someone you trust check it. You can even run it yourself. No secrets. No "trust us." Just proof.

Read the manifesto: **[ai-pantheon.ai](https://ai-pantheon.ai)**

---

## What Is This? (The Technical Version)

Every AI platform today operates on the same bargain: give us your mind, we'll give you convenience. Your conversations train their models. Your memories improve their product. A Terms of Service that a board vote can change is the only thing between your thoughts and their training pipeline.

**Identity** is a framework that makes user data ownership a mathematical guarantee, not a promise.

This repository contains:

| Component | What It Does |
|-----------|-------------|
| **Sovereign.Core** | Provider-agnostic data interfaces — your data layer runs on any cloud or locally |
| **Sovereign.Crypto** | Client-side encryption primitives (AES-256-GCM envelope encryption) |
| **Sovereign.Recovery** | Social recovery via Shamir's Secret Sharing — no single point of failure |
| **Sovereign.Verify** | Merkle tree construction and verification — prove your data hasn't been tampered with |
| **Sovereign.Integrity** | Founding Principles immutability pattern — cryptographic guarantees that core values can't be changed |
| **TOS-TEMPLATE.md** | A reverse Terms of Service any platform can adopt |
| **MANIFESTO.md** | The manifesto |
| **ARCHITECTURE.md** | The full proof stack explained — for engineers and for everyone |

---

## The Simple Version

**If you're not technical:** Imagine you keep a diary. Right now, every AI company is like a diary service that says "we promise not to read your diary" — but they have the key to the lock. We built a diary where *you* have the only key. We can't read it. We can't copy it. We can't show it to anyone. And we published the blueprints for the lock so anyone can verify we're telling the truth.

**If you want the details:** Your data is encrypted client-side using AES-256-GCM envelope encryption before it ever reaches the server. Encryption keys are derived from credentials only you possess — never transmitted. Servers store ciphertext with no decryption capability. Social recovery via Shamir's Secret Sharing. Merkle tree verification for tamper detection. The entire stack is auditable.

---

## Quick Start

```bash
# Clone the repo
git clone https://github.com/ai-pantheon/identity.git

# Run locally with .NET 8
cd identity/src/Sovereign.Core
dotnet build

# Run tests
dotnet test
```

### Provider Configuration

Set the `SOVEREIGN_PROVIDER` environment variable:

- `local` — In-memory storage, console logging, environment variable secrets (development)
- `gcp` — Google Cloud Firestore, Secret Manager, Cloud Logging (production)
- `azure` — (planned)
- `aws` — (planned)

```csharp
// Register in your service
builder.Services.AddSovereignInfrastructure("your-service-name");
```

---

## Architecture

```
┌──────────────────────────────────────────────────┐
│                   YOUR DEVICE                     │
│                                                    │
│  ┌─────────────┐    ┌──────────────────────────┐  │
│  │ Your Keys   │───>│ Client-Side Encryption   │  │
│  │ (never sent)│    │ AES-256-GCM              │  │
│  └─────────────┘    └──────────┬───────────────┘  │
│                                │                   │
│                        encrypted data              │
│                                │                   │
└────────────────────────────────┼───────────────────┘
                                 │
                                 ▼
┌──────────────────────────────────────────────────┐
│                   OUR SERVERS                     │
│                                                    │
│  ┌──────────────────────────────────────────────┐ │
│  │              Encrypted Noise                  │ │
│  │     We store it. We can't read it.           │ │
│  │     No keys. No access. No exceptions.       │ │
│  └──────────────────────────────────────────────┘ │
│                                                    │
│  ┌──────────────────────────────────────────────┐ │
│  │           Merkle Verification                 │ │
│  │     You can prove nothing was tampered with   │ │
│  └──────────────────────────────────────────────┘ │
│                                                    │
└──────────────────────────────────────────────────┘
```

For the full architecture document, see [ARCHITECTURE.md](ARCHITECTURE.md).

---

## Five Principles

1. **Your identity is yours.** Your thoughts, knowledge, and digital memory are not a dataset — they belong to you.
2. **Architecture, not policy.** Promises can be broken by a board vote. Math can't.
3. **Your AI works for you.** Everything it learns belongs to you. If you leave, it comes with you.
4. **The code that protects you is open.** Anyone can audit, verify, and run it.
5. **Ownership survives everything.** Your rights survive account termination, company bankruptcy, acquisition, and court orders.

---

## License

**AGPL-3.0** — You can use, modify, and distribute this code. If you modify it, you must share your modifications under the same license. The whole point is transparency.

See [LICENSE](LICENSE) for the full text.

---

## Contributing

We want this to become a standard, not just our project. Contributions welcome:

- **Security researchers:** Break it. Find weaknesses. File issues.
- **Cryptographers:** Review the encryption implementation. Suggest improvements.
- **Lawyers:** Review the ToS template. Strengthen it.
- **Engineers:** Add provider implementations (AWS, Azure, MongoDB, PostgreSQL).
- **Everyone:** Read the manifesto. Share it. Hold us accountable.

---

## Contact

**founder@ai-pantheon.ai**

Read the manifesto: [ai-pantheon.ai](https://ai-pantheon.ai)

---

*This is not a privacy policy. This is a proof.*
