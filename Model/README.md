# Model

`Model` contains the typed stateless calculation contracts, atomic validation, thread-safe cumulative usage counters that the Service can snapshot and restore, installed-model provenance, and `EarthMagneticFieldEvaluator`.

The evaluator loads WMM2025 and IGRF14 once, accepts WGS84 radians, positive-down ellipsoidal depth, and explicit UTC, and returns north-east-down SI teslas plus horizontal/total intensity and declination/magnetic dip in radians. GeographicLib boundary conversions are private.

Validation covers coordinates, finite depth, model-specific height/depth and UTC ranges, explicit zero UTC offset, supported model selection, empty/oversized batches, and null samples. Results preserve request order.

Author: Eric Cayeux

Company: NORCE Research

The Model references SemanticCatalogue 0.16.0 from NuGet. Semantic attributes declare concepts, roles and references on engineering DTOs; they do not change serialization or validation. UTC instants have no duration quantity. SHA-256 digests share one noun with distinct source-file roles.
