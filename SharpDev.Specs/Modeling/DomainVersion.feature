Feature: DomainVersion
	Allows to represent a version stamp

@modeling
Scenario Outline: Create new major version stamp
	Given A new major version is created with <major>
	When The version is printed
	Then The printed version looks like "<printed>"
	Examples: 
	| major | printed |
	| 0     | 0       |
	| 1     | 1       |
	| 9     | 9       |
	| 17    | 17      |

Scenario Outline: Create new major.minor version stamp
	Given A new major.minor version is created with <major> and <minor>
	When The version is printed
	Then The printed version looks like "<printed>"
	Examples: 
	| major | minor | printed |
	| 0     | 0     | 0.0     |
	| 0     | 1     | 0.1     |
	| 1     | 0     | 1.0     |
	| 1     | 1     | 1.1     |
	| 9     | 2     | 9.2     |
	| 17    | 3     | 17.3    |

Scenario Outline: Create new major.minor.revision version stamp
	Given A new major.minor.revision version is created with <major>, <minor> and <revision>
	When The version is printed
	Then The printed version looks like "<printed>"
	Examples: 
	| major | minor | revision | printed |
	| 0     | 0     | 2        | 0.0.2   |
	| 0     | 1     | 3        | 0.1.3   |
	| 1     | 0     | 4        | 1.0.4   |
	| 1     | 1     | 5        | 1.1.5   |
	| 9     | 2     | 6        | 9.2.6   |
	| 17    | 3     | 7        | 17.3.7  |

Scenario Outline: Create new major.minor.revision.build version stamp
	Given A new major.minor.revision.build version is created with <major>, <minor>, <revision> and <build>
	When The version is printed
	Then The printed version looks like "<printed>"
	Examples: 
	| major | minor | revision | build | printed     |
	| 0     | 0     | 2        | 9876  | 0.0.2.9876  |
	| 0     | 1     | 3        | 5432  | 0.1.3.5432  |
	| 1     | 0     | 4        | 2468  | 1.0.4.2468  |
	| 1     | 1     | 5        | 12    | 1.1.5.12    |
	| 9     | 2     | 6        | 98765 | 9.2.6.98765 |
	| 17    | 3     | 7        | 0     | 17.3.7.0    |
