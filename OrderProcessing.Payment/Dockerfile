FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["OrderProcessing.Payment/OrderProcessing.Payment.csproj", "OrderProcessing.Payment/"]
COPY ["OrderProcessing.Api/OrderProcessing.Api.csproj", "OrderProcessing.Api/"]
COPY ["OrderProcessing.Shared/OrderProcessing.Shared.csproj", "OrderProcessing.Shared/"]
RUN dotnet restore "OrderProcessing.Payment/OrderProcessing.Payment.csproj"
COPY . .
RUN dotnet publish "OrderProcessing.Payment/OrderProcessing.Payment.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "OrderProcessing.Payment.dll"]
