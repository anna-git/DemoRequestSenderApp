#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 84

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

# Download the latest version of the tracer but don't install yet
RUN TRACER_VERSION=$(curl -s \https://api.github.com/repos/DataDog/dd-trace-dotnet/releases/latest | grep tag_name | cut -d '"' -f 4 | cut -c2-) \
    && curl -Lo /tmp/datadog-dotnet-apm.deb https://github.com/DataDog/dd-trace-dotnet/releases/download/v${TRACER_VERSION}/datadog-dotnet-apm_${TRACER_VERSION}_amd64.deb


WORKDIR /src
COPY ["DemoRequestSenderApp/DemoRequestSenderApp.csproj", "DemoRequestSenderApp/"]
RUN dotnet restore "DemoRequestSenderApp/DemoRequestSenderApp.csproj"
COPY . .
WORKDIR "/src/DemoRequestSenderApp"
RUN dotnet build "DemoRequestSenderApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DemoRequestSenderApp.csproj" -c Release -o /app/publish

FROM base AS final


# Copy the tracer from build target
COPY --from=build /tmp/datadog-dotnet-apm.deb /tmp/datadog-dotnet-apm.deb
# Install the tracer
RUN mkdir -p /opt/datadog \
    && mkdir -p /var/log/datadog \
    && dpkg -i /tmp/datadog-dotnet-apm.deb \
    && rm /tmp/datadog-dotnet-apm.deb

# Enable the tracer
ENV CORECLR_ENABLE_PROFILING=1
ENV CORECLR_PROFILER={846F5F1C-F9AE-4B07-969E-05C26BC060D8}
ENV CORECLR_PROFILER_PATH=/opt/datadog/Datadog.Trace.ClrProfiler.Native.so
ENV DD_DOTNET_TRACER_HOME=/opt/datadog
ENV DD_INTEGRATIONS=/opt/datadog/integrations.json
ENV DD_APPSEC_ENABLED=true
# ENV DD_APPSEC_RULES=

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DemoRequestSenderApp.dll"]