FROM mysql:8.3.0
EXPOSE 3308
COPY ./RestWithASPNET/db/migrations/ /home/database/
COPY ./RestWithASPNET/db/dataset/ /home/database/
COPY ./RestWithASPNET/ci/init_database.sh /docker-entrypoint-initdb.d/init_database.sh