# Configuration Guide <br/> Quotes Microservice

Configuration structure used by this module follows the 
[standard configuration](https://github.com/pip-services/pip-services/blob/master/usage/Configuration.md) 
structure.

Example **config.yml** file:

```yaml
- descriptor: "pip-services-container:container-info:default:default:1.0"
  name: "pip-services-quotes"
  description: "Quotes microservice"

- descriptor: "pip-services-commons:logger:console:default:1.0"
  level: "trace"

- descriptor: "pip-services-quotes:persistence:file:default:1.0"
  path: "./data/quotes.json"

- descriptor: "pip-services-quotes:controller:default:default:1.0"

- descriptor: "pip-services-quotes:service:http:default:1.0"
  connection:
    protocol: "http"
    host: "0.0.0.0"
    port: 3000
```
