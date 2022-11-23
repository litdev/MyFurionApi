# 概述

    基于 [Furion](https://furion.baiqian.ltd/docs) 框架，结合现有的业务，对框架进行业务性完善，已达到开箱即用的程度

ORM使用SqlSugar

# 开发中使用本机IIS

在本地IIS中创建一个网站，目录指向 `/MyFurionApi.Web.Entry`，端口设置 `5010`，访问域名即 `http://localhost:5010/doc/index.html`

> 此功能需要到 `Visual Studio Installer`程序中勾选`开发时间 IIS 支持`

# Docker部署

两种方式

#### A. 发布后构建

1. 创建Publis发布配置文件
   
   对项目`MyFurionApi.Web.Entry` 右键 `发布` 创建一个本地的发布配置文件 `FolderProfile.pubxml` 按需选择发布配置，点击发布后，在`bin\Release\net6.0\publish\` 输出发布后的文件

2. 在输出文件夹创建 `Dockerfile` 文件，内容如下：
   
   ```bash
   # 构建
   docker build -t my-furion-api .
   # 运行
   docker run --name my-furion-api-prod -p 5011:80 -v D:\Docker\Volumes\MyFurionApi\appsettings.json:/app/appsettings.json -v D:\Docker\Volumes\MyFurionApi\wwwroot:/app/wwwroot -v D:\Docker\Volumes\MyFurionApi\logs:/app/logs -e ASPNETCORE_ENVIRONMENT="Production" -e TZ=Asia/Shanghai -d my-furion-api
   ```
   
   > 注意docker run中将配置文件和日志目录映射到容器中，宿主机修改了配置文件，需要重启容器才会生效

#### B. 编译+构建+发布

在`MyFurionApi.sln`同目录下创建`Dockerfile` 文件，内容如下：

```bash
#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM registry.cn-zhangjiakou.aliyuncs.com/litdev-dotnet/aspnet-with-libgdiplus:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MyFurionApi.Web.Entry/MyFurionApi.Web.Entry.csproj", "MyFurionApi.Web.Entry/"]
COPY ["MyFurionApi.Web.Core/MyFurionApi.Web.Core.csproj", "MyFurionApi.Web.Core/"]
COPY ["MyFurionApi.Application2/MyFurionApi.Application2.csproj", "MyFurionApi.Application2/"]
COPY ["MyFurionApi.Core/MyFurionApi.Core.csproj", "MyFurionApi.Core/"]
COPY ["MyFurionApi.Application/MyFurionApi.Application.csproj", "MyFurionApi.Application/"]
RUN dotnet restore "MyFurionApi.Web.Entry/MyFurionApi.Web.Entry.csproj"
COPY . .
WORKDIR "/src/MyFurionApi.Web.Entry"
RUN dotnet build "MyFurionApi.Web.Entry.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyFurionApi.Web.Entry.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyFurionApi.Web.Entry.dll"]
```

在`Dockerfile`目录下执行如下命令：

```bash
# 构建
docker build -t my-furion-api .
# 运行
docker run --name my-furion-api-prod -p 5011:80 -v D:\Docker\Volumes\MyFurionApi\appsettings.json:/app/appsettings.json -v D:\Docker\Volumes\MyFurionApi\wwwroot:/app/wwwroot -v D:\Docker\Volumes\MyFurionApi\logs:/app/logs -e ASPNETCORE_ENVIRONMENT="Production" -e TZ=Asia/Shanghai -d my-furion-api
```

 批处理： 

```bash
# 停止容器
docker stop my-furion-api-prod
# 删除容器
docker rm my-furion-api-prod
# 删除镜像
docker rmi my-furion-api
# 删除名称为none的镜像
docker rmi $(docker images | grep "none" | awk '{print $3}')
# 构建
docker build -t my-furion-api .
# 运行
docker run --name my-furion-api-prod -p 5011:80 -v D:\Docker\Volumes\MyFurionApi\appsettings.json:/app/appsettings.json -v D:\Docker\Volumes\MyFurionApi\wwwroot:/app/wwwroot -v D:\Docker\Volumes\MyFurionApi\logs:/app/logs -e ASPNETCORE_ENVIRONMENT="Production" -e TZ=Asia/Shanghai -d my-furion-api
```

不同系统下访问宿主机IP：

`Windows`：`192.168.65.2`

`Linux`：`172.17.0.1`