# .NET Core Web API Project

This project is for practicing .NET Core Web API development. It uses Docker Compose to launch the whole backend stack, so please make sure that [Docker](https://www.docker.com/) is installed.

## Development

To start PostgreSQL in Docker for local development and testing, make sure to provide the following variables:

Recommend using [direnv](https://github.com/direnv/direnv) for management of environment variables.

```
POSTGRES_HOST=postgres
POSTGRES_PORT=5432
POSTGRES_USER=
POSTGRES_PASSWORD=
POSTGRES_DB=
```

To start the whole stack, run:
	
```
$ make up
```

To take down the stack, run:

```
$ make down
```

To view logs for a specific service, use `logs-{service}`. You can also customize the number of log lines to display with the `log_lines` argument:
	
```
$ make logs-api log_lines=50
```

Swagger UI can be found at [http://127.0.0.1:5000/swagger/index.html](http://127.0.0.1:5000/swagger/index.html). You can use it to test the API endpoints and explore the API documentation.
