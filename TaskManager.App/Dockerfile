# Stage 1
FROM node:18-bullseye-slim as node
WORKDIR /app
COPY . .
RUN npm ci
RUN npm run build --prod

# Stage 2
FROM nginx:1.23-alpine-slim
COPY --from=node /app/dist/tasks-app /usr/share/nginx/html