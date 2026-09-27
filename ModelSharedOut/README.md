# ModelSharedOut

Generates `EarthMagneticFieldMergedModel.cs` and the merged service JSON Schema from `json-schemas/EarthMagneticFieldFullName.json`. `PseudoConstructors.cs` provides hand-maintained conveniences for tests and WebPages. CI regenerates all artifacts and rejects uncommitted contract differences.

Author: Eric Cayeux

Company: NORCE Research

Magnetic dip uses the `MagneticDip` result property and the label “Magnetic dip”. It is in SI radians, positive downward from horizontal, and nullable when total magnetic flux density is zero. Service and client releases must agree on this property name.
