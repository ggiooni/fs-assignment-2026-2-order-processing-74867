FROM node:20-alpine AS build
WORKDIR /app
COPY OrderProcessing.ReactAdmin/package*.json ./
RUN npm install
COPY OrderProcessing.ReactAdmin/ .
ENV VITE_API_URL=http://localhost:5000
RUN npm run build

FROM nginx:alpine
COPY --from=build /app/dist /usr/share/nginx/html
COPY OrderProcessing.ReactAdmin/nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
