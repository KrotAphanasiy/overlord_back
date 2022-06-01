FROM tsgkadot/docker-doxygen as build
WORKDIR /build

ADD Doxyfile /build/
COPY ./src /build/src

RUN doxygen /build/Doxyfile

###############

FROM nginx:stable-alpine as result
EXPOSE 80
ENTRYPOINT [ "/docs-start.sh" ]

RUN apk add --update --no-cache --repository http://dl-3.alpinelinux.org/alpine/edge/testing/ --allow-untrusted \
    curl jq && \
    rm -rf /tmp/* /var/cache/apk/*

COPY --from=build /build/docs/html /usr/share/nginx/html
ADD ./deploy/docs-start.sh /
RUN chmod +x /docs-start.sh
