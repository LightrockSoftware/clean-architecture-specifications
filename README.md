# Order Processing – Clean Architecture with EF Core & Specifications

This project demonstrates how to replace the traditional
"Repository Pattern over EF Core" with:

- Clean Architecture
- Specification Pattern
- EF Core as an Infrastructure concern

## Why?

EF Core already provides repository and unit-of-work behavior.
Wrapping it with query-heavy repositories leads to:

- Method explosion
- Leaky abstractions
- Poor query reuse

Specifications solve this by making query intent explicit.

---

## Project Structure