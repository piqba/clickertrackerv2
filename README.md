# Clicker Tracker



## PoC to fetch CLicks events from a web page




### Tools 


- [redpanda-console](http://localhost:8080/overview) defined in docker compose file folder `infra`
- Postgresql
- jagger
- prometheus
- otel-collector









#### TODO:


- IExceptionHandler & AddExceptionHandler
- Customs Errors
- Add more OTEL Activities
- Middleware to extract API-KEY and check in redis database.
  - create worker process to populate redis hashSet (api-key:app-name)