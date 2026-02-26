# ── Build ──
FROM node:22-alpine AS build
WORKDIR /app
COPY vue-project/package.json vue-project/package-lock.json ./
RUN npm ci
COPY vue-project/ ./
RUN npm run build-only

# ── Serve ──
FROM nginx:alpine
COPY --from=build /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80