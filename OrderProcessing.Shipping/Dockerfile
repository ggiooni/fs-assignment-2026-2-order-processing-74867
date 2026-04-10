FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["OrderProcessing.Shipping/OrderProcessing.Shipping.csproj", "OrderProcessing.Shipping/"]
COPY ["OrderProcessing.Api/OrderProcessing.Api.csproj", "OrderProcessing.Api/"]
COPY ["OrderProcessing.Shared/OrderProcessing.Shared.csproj", "OrderProcessing.Shared/"]
RUN dotnet restore "OrderProcessing.Shipping/OrderProcessing.Shipping.csproj"
COPY . .
RUN dotnet publish "OrderProcessing.Shipping/OrderProcessing.Shipping.csproj" -c Release -o /app /p:ErrorOnDuplicatePublishOutputFiles=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "OrderProcessing.Shipping.dll"]
