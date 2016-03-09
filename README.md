# SharpDev

Another DDD-CQRS-ES framework! Good luck to us ;)

## Discussion points
* Use of DSL specifically to model concepts of DDD, CQRS and ES, allowing to evolve the system in production/staging/testing/development without requiring a development environment like VS
* Define a unified runtime framework for executing a CQRS system
* Use Akka.net for clustering without strong dependencies
* Use Azure for deployment without strong dependencies
* Allow local, on-premisses and cloud-based deployments
* Static/dynamic Code generation for most of the boilerplate, to allow the developer to focus on business logic
