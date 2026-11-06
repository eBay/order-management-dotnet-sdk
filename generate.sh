#!/usr/bin/env bash

# Copyright 2026 eBay Inc.
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
#     http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.

set -euo pipefail

echo "Pinning Kiota to 1.35.0..."
dotnet tool update microsoft.openapi.kiota --local --version 1.35.0 --allow-downgrade

echo "Restoring tools..."
dotnet tool restore

echo "Regenerating REST clients via Kiota..."
rm -rf ./EBay.OrderManagement/Generated

dotnet kiota generate \
  -l csharp \
  -d specs/openapi/fulfillment.json \
  -c FulfillmentApiClient \
  -n EBay.OrderManagement.Generated.Fulfillment \
  -o EBay.OrderManagement/Generated/Fulfillment \
  --ebc --co

dotnet kiota generate \
  -l csharp \
  -d specs/openapi/logistics.json \
  -c LogisticsApiClient \
  -n EBay.OrderManagement.Generated.Logistics \
  -o EBay.OrderManagement/Generated/Logistics \
  --ebc --co

dotnet kiota generate \
  -l csharp \
  -d specs/openapi/post-order-api.json \
  -c PostOrderApiApiClient \
  -n EBay.OrderManagement.Generated.PostOrderApi \
  -o EBay.OrderManagement/Generated/PostOrderApi \
  --ebc --co

echo "Adding license headers to generated files..."
./add-license-headers.sh ./EBay.OrderManagement/Generated

echo "Building (compile check)..."
DOTNET_ROLL_FORWARD=LatestMajor dotnet build EBay.OrderManagement/EBay.OrderManagement.csproj

echo "Done."
