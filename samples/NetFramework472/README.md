# .NET Framework 4.7.2 sample

A WinForms app that uses `EBay.OrderManagement` the way an older .NET Framework application typically consumes NuGet packages:

| Setup | How the sample does it |
| --- | --- |
| .NET Framework 4.7.2 | `TargetFrameworkVersion` v4.7.2, `supportedRuntime` 4.7.2 in `App.config` |
| C# 7.3 | `LangVersion` 7.3; warnings are errors |
| `packages.config`, classic MSBuild | Old-style `.csproj` with `HintPath` references; **every** transitive package listed by hand in `packages.config` |
| The app's own JSON library | Newtonsoft.Json, used for the order-details pane; the SDK itself doesn't depend on Newtonsoft |
| Hand-written binding redirects | `AutoGenerateBindingRedirects` is off; `App.config` has the seven redirects the SDK needs. `--smoke` loads every referenced assembly through them, so a missing one fails |

## What it shows

- **Production / Sandbox** switch. Sandbox points every API at `*.sandbox.ebay.com`; those hosts are trusted by default.
- **Load orders**: calls the Fulfillment REST API (`GetOrdersAsync`, the last N days) and the GraphQL `orders` query **concurrently**, and joins them by order id. REST gives fulfillment status and total; GraphQL gives the order state and payment state, read from the GraphQL union types with C# 7 type patterns.
- **Select a row**: fetches the full order (`GetOrderAsync`) and renders it with the app's own Newtonsoft.Json.
- **GraphQL URL (optional)**: overrides the GraphQL endpoint. A host that isn't an eBay default is added to `AllowedHosts` explicitly, which is how a proxy would be trusted. There's no documented public sandbox GraphQL endpoint, so the sandbox default (`https://api.sandbox.ebay.com/graphql`) may need changing.

You need a **user access token** with the `sell.fulfillment` scope (eBay developer portal → User Tokens), for the environment you select.

## Run it (Windows)

```bat
REM from the repository root
dotnet pack EBay.OrderManagement -c Release -o artifacts\packages
cd samples\NetFramework472
nuget restore OrderManagementSample.sln
msbuild OrderManagementSample.sln /p:Configuration=Release
OrderManagementSample\bin\Release\OrderManagementSample.exe
```

The first step is only needed until 0.4.0 is on nuget.org; `NuGet.config` reads the local `artifacts/packages` feed first.

`OrderManagementSample.exe --smoke` runs an offline end-to-end check instead of the UI: no network or token, canned eBay responses through the SDK's `(config, HttpMessageHandler)` constructor. It covers both environments, the REST + GraphQL join, Newtonsoft rendering, that the token reaches only the configured hosts, and that an untrusted GraphQL host is rejected. It prints the assembly versions actually loaded and exits non-zero on failure. CI runs it on `windows-latest` (`sample-netfx` job in `.github/workflows/validate.yml`).

## Dependencies on .NET Framework

The SDK requires **System.Text.Json >= 10.0.0** on .NET Framework. It follows the Azure SDK rule of aligning .NET runtime libraries with the oldest supported .NET LTS, which is .NET 10 from November 2026. Kiota itself accepts anything from 6.0.10, but 6.x has been out of support since November 2024.

At the SDK's minimum versions, `packages.config` lists 23 packages besides the app's own:

| Package | Version |
| --- | --- |
| EBay.OrderManagement | 0.4.0 |
| Microsoft.Kiota.Abstractions, .Bundle, .Http.HttpClientLibrary, .Serialization.Form, .Serialization.Json, .Serialization.Multipart, .Serialization.Text | 1.22.0 |
| Std.UriTemplate | 2.0.8 |
| Microsoft.IO.RecyclableMemoryStream | 3.0.1 |
| Microsoft.Extensions.DependencyInjection.Abstractions | 6.0.0 |
| System.Diagnostics.DiagnosticSource | 6.0.0 |
| System.Net.Http.WinHttpHandler | 6.0.0 |
| System.Text.Json, System.Text.Encodings.Web, System.IO.Pipelines, Microsoft.Bcl.AsyncInterfaces | 10.0.0 |
| System.Runtime.CompilerServices.Unsafe | 6.1.2 |
| System.Memory | 4.6.3 |
| System.Threading.Tasks.Extensions | 4.6.3 |
| System.Buffers, System.Numerics.Vectors, System.ValueTuple | 4.6.1 |

None of these minimums has a known advisory. Newer versions work too (CI also tests the latest ones); they would change the redirect versions below.

### Upgrading from System.Text.Json 4.7.2

An app that already uses System.Text.Json 4.7.2 has these packages from it, and installing the SDK moves them up:

| Package | With System.Text.Json 4.7.2 | With the SDK (minimum) | Assembly version |
| --- | --- | --- | --- |
| System.Text.Json | 4.7.2 | 10.0.0 | 4.0.1.2 → 10.0.0.0 |
| System.Text.Encodings.Web | 4.7.1 | 10.0.0 | 4.0.5.0 → 10.0.0.0 |
| Microsoft.Bcl.AsyncInterfaces | 1.1.0 | 10.0.0 | 1.0.0.0 → 10.0.0.0 |
| System.Runtime.CompilerServices.Unsafe | 4.7.1 | 6.1.2 | 4.0.6.0 → 6.0.3.0 |
| System.Memory | 4.5.4 | 4.6.3 | 4.0.1.1 → 4.0.5.0 |
| System.Buffers | 4.5.1 | 4.6.1 | 4.0.3.0 → 4.0.5.0 |
| System.Numerics.Vectors | 4.5.0 | 4.6.1 | 4.1.4.0 → 4.1.6.0 |
| System.Threading.Tasks.Extensions | 4.5.4 | 4.6.3 | 4.2.0.1 → 4.2.4.0 |
| System.ValueTuple | 4.5.0 | 4.6.1 | 4.0.3.0 → *built into .NET Framework* |

System.Text.Json 4.7.2 has been out of support since December 2022, and the System.Text.Encodings.Web 4.7.1 it brings in has a critical advisory (CVE-2021-26701), so this upgrade is worth doing regardless. Code that calls System.Text.Json directly should be retested.

### Binding redirects

With the minimum versions, exactly seven redirects are needed (see `OrderManagementSample/App.config`):

| Assembly | Redirect to |
| --- | --- |
| Microsoft.Bcl.AsyncInterfaces | 10.0.0.0 |
| System.Buffers | 4.0.5.0 |
| System.Memory | 4.0.5.0 |
| System.Runtime.CompilerServices.Unsafe | 6.0.3.0 |
| System.Text.Encodings.Web | 10.0.0.0 |
| System.Text.Json | 10.0.0.0 |
| System.Threading.Tasks.Extensions | 4.2.4.0 |

There must be **no** System.ValueTuple redirect; remove one if the app has it. .NET Framework 4.7.1+ provides System.ValueTuple, and Microsoft's System.ValueTuple 4.6.1 package says any redirect for it must be removed, because its assembly version (4.0.5.0) is higher than the one the framework provides. MSBuild still reports one MSB3276 warning suggesting a ValueTuple redirect; ignore that one. For any other assembly, MSB3276/MSB3277 means a redirect is missing.
