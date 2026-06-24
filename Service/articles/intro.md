---
title: "How to use the EarthMagneticField microservice?"
output: html_document
---

Typical Usage
===
1. Upload a new EarthMagneticFieldCalculationOrder using the `Post` web api method.
2. Call the `Get` method with the identifier of the uploaded EarthMagneticFieldCalculationOrder as argument. 
The return Json object contains the EarthMagneticFieldCalculationOrder description.
3. Optionally send a `Delete` request with the identifier of the EarthMagneticFieldCalculationOrder in order to delete the EarthMagneticFieldCalculationOrder if you do not 
want to keep the EarthMagneticFieldCalculationOrder uploaded on the microservice.


