# Stage 1: Compile and Build angular codebase

# Use official node image as the base image
FROM node:19.8.1-alpine3.17 as build

# Set the working directory
WORKDIR /usr/local/app

# Add the source code to app
COPY ./ /usr/local/app/

# Remove version error messages
RUN npm config set update-notifier false

# Install all the dependencies
RUN npm install

# Generate the build of the application
RUN npm run build


# Stage 2: Serve app with nginx server

# Use official nginx image as the base image
FROM nginx:latest

# Copy the build output to replace the default nginx contents.
COPY --from=build /usr/local/app/dist/tasks-app /usr/share/nginx/html

# Expose port 80
EXPOSE 80