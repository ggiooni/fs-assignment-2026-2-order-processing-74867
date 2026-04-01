FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["OrderProcessing.BlazorUI/OrderProcessing.BlazorUI.csproj", "OrderProcessing.BlazorUI/"]
COPY ["OrderProcessing.Shared/OrderProcessing.Shared.csproj", "OrderProcessing.Shared/"]
RUN dotnet restore "OrderProcessing.BlazorUI/OrderProcessing.BlazorUI.csproj"
COPY . .
RUN dotnet publish "OrderProcessing.BlazorUI/OrderProcessing.BlazorUI.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app .
EXPOSE 8080
ENTRYPOINT ["dotnet", "OrderProcessing.BlazorUI.dll"]
