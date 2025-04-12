# **N-Tier Architecture**

[![Build](https://github.com/nuyonu/N-Tier-Architecture/actions/workflows/build.yml/badge.svg?branch=main)](https://github.com/nuyonu/N-Tier-Architecture/actions/workflows/build.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=nuyonu_N-Tier-Architecture&metric=alert_status)](https://sonarcloud.io/dashboard?id=nuyonu_N-Tier-Architecture)
![](https://camo.githubusercontent.com/deab10366c6377e3d4cc454a26f96225e2cc196214b129b95c9d5284207b64d7/68747470733a2f2f696d672e736869656c64732e696f2f7374617469632f76313f6c6162656c3d254630253946253843253946266d6573736167653d496625323055736566756c267374796c653d7374796c653d666c617426636f6c6f723d424334453939)
![](https://api.visitorbadge.io/api/VisitorHit?user=nuyonu&repo=N-Tier-Architecture&countColor=%237B1E7A)

This is a n-layer architecture based on [Common web application architectures][common-web-architectures]. The technologies used can be found below. It will be updated to the latest versions, depending on how stable they will be.

## **Introduction**

<div align="center">

[![SonarCloud](https://sonarcloud.io/images/project_badges/sonarcloud-white.svg)](https://sonarcloud.io/dashboard?id=nuyonu_N-Tier-Architecture)

[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=nuyonu_N-Tier-Architecture&metric=sqale_rating)](https://sonarcloud.io/dashboard?id=nuyonu_N-Tier-Architecture) 
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=nuyonu_N-Tier-Architecture&metric=reliability_rating)](https://sonarcloud.io/dashboard?id=nuyonu_N-Tier-Architecture) 
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=nuyonu_N-Tier-Architecture&metric=security_rating)](https://sonarcloud.io/dashboard?id=nuyonu_N-Tier-Architecture) 
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=nuyonu_N-Tier-Architecture&metric=coverage)](https://sonarcloud.io/dashboard?id=nuyonu_N-Tier-Architecture)

[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=nuyonu_N-Tier-Architecture&metric=bugs)](https://sonarcloud.io/dashboard?id=nuyonu_N-Tier-Architecture)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=nuyonu_N-Tier-Architecture&metric=code_smells)](https://sonarcloud.io/dashboard?id=nuyonu_N-Tier-Architecture)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=nuyonu_N-Tier-Architecture&metric=duplicated_lines_density)](https://sonarcloud.io/dashboard?id=nuyonu_N-Tier-Architecture)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=nuyonu_N-Tier-Architecture&metric=vulnerabilities)](https://sonarcloud.io/dashboard?id=nuyonu_N-Tier-Architecture)

</div>

---

## Technologies
- .NET 9
- ASP.NET Core 9
- Swagger (Documentation)
- Entity Framework Core (SQL Server)
- ASP.NET Core Identity (SQL Server)
- [Mapster](https://github.com/MapsterMapper/Mapster)
- [FluentValidation](https://docs.fluentvalidation.net/en/latest/)
- [NUnit (Integration tests)](https://nunit.org/)
- [XUnit (Unit tests)](https://xunit.net/)
- [FluentAssertion (Testing projects)](https://fluentassertions.com/)
- [NBuilder (Testing projects)](https://github.com/nbuilder/nbuilder)

---

## Dependencies between projects

<div align="center">

<img src="https://raw.githubusercontent.com/nuyonu/N-Tier-Architecture/master/github/images/N-Tier-Dependencies.png" alt="drawing" width="400"/>
    
</div>

---

## **Getting Started**

<!-- Before you begin, please read the [requirements](#requirements).  -->

A quick method to use the exposed solution is to download a copy of this project, if you meet all the requirements, the project will run without any problems and can be used from the first second.

## Database migrations

Migrations will be applied automatically. If you want to add new migrations to be applied to over the database, you will need to run the command below in the root folder

```c#
dotnet ef migrations add Migration-Name --project N-Tier.DataAccess -o Persistence/Migrations --startup-project N-Tier.API
```

<!-- ## **Maintainers** -->
<!-- // TODO -->

## **Support**

If you are having problems, please let me know by raising a [new issue](https://github.com/nuyonu/N-Tier-Architecture/issues/new/choose).


[common-web-architectures]: https://docs.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures
