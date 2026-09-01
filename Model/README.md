# Model

`Model` contains the typed stateless calculation contracts, atomic validation, thread-safe cumulative usage counters that the Service can snapshot and restore, installed-model provenance, and `EarthMagneticFieldEvaluator`.

The evaluator loads WMM2025 and IGRF14 once, accepts WGS84 radians, positive-down ellipsoidal depth, and explicit UTC, and returns north-east-down SI teslas plus horizontal/total intensity and declination/inclination in radians. GeographicLib boundary conversions are private.

Validation covers coordinates, finite depth, model-specific height/depth and UTC ranges, explicit zero UTC offset, supported model selection, empty/oversized batches, and null samples. Results preserve request order.

Author: Eric Cayeux

Company: NORCE Research
