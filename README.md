# HealthCoreX

HealthCoreX is a source-available, Kubernetes-first healthcare payer
administration platform. It is built for teams modernizing claims, benefits,
eligibility, prior authorization, FHIR interoperability, and X12 operations
without treating a legacy Core Administration Processing System as the only
place business logic can live.

## Executive Summary

Health plans need modern APIs, event-driven operations, auditable adjudication,
and CMS-0057-F interoperability. Legacy CAPS platforms such as Facets, QNXT, and
HealthEdge remain operationally important, but they were not designed around
Kubernetes, FHIR R4, X12 event streams, or continuous benchmark evidence.

HealthCoreX is designed as a cloud-native payer platform that can be
deployed alongside existing systems, used to validate specific workloads, and
progressively expanded. Current evidence is strongest around local Kubernetes
claims adjudication, workflow scoring, pended-claim observability, and operator
console inspection through the Million Claim Challenge.

## Who It Is For

- Health plan engineering teams evaluating CAPS modernization paths.
- Payer platform architects working with claims, benefits, eligibility, and
  prior authorization systems.
- Healthcare interoperability teams implementing CMS-0057-F, FHIR R4, and X12.
- Contributors who want to work on production-oriented healthcare platform
  infrastructure.

## What Makes It Different

- **Kubernetes-first:** services, jobs, workflows, and local benchmark runs are
  designed around containerized operation.
- **Evidence-first:** the Million Claim Challenge publishes reproducible command
  lines, run summaries, validation outcomes, and raw artifacts instead of only
  marketing claims.
- **API-first:** payer operations are exposed through service APIs, a Blazor
  portal, and FHIR/X12 integration surfaces.
- **Event-oriented:** claims and operational workflows are structured for
  asynchronous processing, durable audit trails, and future streaming analytics.
- **Truthful scoring:** paid, denied, pended, mismatched, unsupported, platform
  failures, false pends, and payment deltas are separated.

## Key Features

| Area | Current focus |
| --- | --- |
| Claims adjudication | Professional, institutional, and dental synthetic claims; workflow scoring; claim detail views; mass adjudication console |
| Benefit administration | Declarative benefit models, cost sharing, accumulators, service-category mapping, and plan versioning |
| Pricing and edits | Fee schedules, NCCI/MUE checks, claims scrubbing, COB, provider network checks, and prior-auth rules |
| Interoperability | FHIR R4 projections, X12 parsing/processing, terminology lookup, and CMS-0057-F readiness docs |
| Operations portal | Claims search, claim detail, mass adjudication runs, EDI transaction history (834/837), dashboards, work queues, and administrative surfaces |
| Deployment | Docker Compose, Kubernetes manifests, GitHub Actions, and deployment documentation |
| Benchmarks | 5K, 10K, 50K, 100K, and full 1,000,000-claim Million Claim Challenge evidence packets |
