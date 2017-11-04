# HTTP Protocol (version 1) <br/> Quotes Microservice

Quotes microservice implements a HTTP compatible API, that can be accessed on configured port.
All input and output data is serialized in JSON format. Errors are returned in [standard format]().

* [MultiStringV1 class](#class1)
* [QuoteV1 class](#class2)
* [DataPage<QuoteV1> class](#class3)
* [POST /quotes/get_quotes](#operation1)
* [POST /quotes/get_random_quote](#operation2)
* [POST /quotes/get_quote_by_id](#operation3)
* [POST /quotes/create_quote](#operation4)
* [POST /quotes/update_quote](#operation5)
* [POST /quotes/delete_quote_id](#operation6)

## Data types

### <a name="class1"></a> MultiStringV1 class

String that contains versions in multiple languages

**Properties:**
- en: string - English version of the string
- sp: string - Spanish version of the string
- de: string - German version of the string
- fr: string - Franch version of the string
- pt: string - Portuguese version of the string
- ru: string - Russian version of the string
- .. - other languages can be added here

### <a name="class2"></a> QuoteV1 class

Represents an inspirational quote

**Properties:**
- id: string - unique quote id
- text: MultiString - quote text in different languages
- author: MultiString - name of the quote author in different languages
- status: string - editing status of the quote: 'new', 'writing', 'translating', 'completed' (default: 'new')
- tags: [string] - (optional) search tags that represent topics associated with the quote
- all_tags: [string] - (read only) explicit and hash tags in normalized format for searching  

### <a name="class3"></a> DataPage<QuoteV1> class

Represents a paged result with subset of requested quotes

**Properties:**
- data: [Quote] - array of retrieved Quote page
- count: int - total number of objects in retrieved resultset

## Operations

### <a name="operation1"></a> Method: 'POST', route '/quotes/get_quotes'

Retrieves a collection of quotes according to specified criteria

**Request body:** 
- correlation_id: string - (optional) unique id that identifies distributed transaction
- filter: Object
  - tags: string - (optional) a comma-separated list of tags with topic names
  - status: string - (optional) quote editing status
  - author: string - (optional) author name in any language 
- paging: Object
  - skip: int - (optional) start of page (default: 0). Operation returns paged result
  - take: int - (optional) page length (max: 100). Operation returns paged result

**Response body:**
Array of Quote objects, DataPage<QuoteV1> object is paging was requested or error

### <a name="operation2"></a> Method: 'POST', route '/quotes/get\_random\_quote'

Retrieves a random quote from filtered resultset

**Request body:** 
- correlation_id: string - (optional) unique id that identifies distributed transaction
- filter: Object
  - tags: string - (optional) a comma-separated list of tags with topic names
  - status: string - (optional) quote editing status
  - author: string - (optional) author name in any language 

**Response body:**
Random Quote object, null if object wasn't found or error 

### <a name="operation3"></a> Method: 'POST', route '/quotes/get\_quote\_by_id'

Retrieves a single quote specified by its unique id

**Request body:** 
- correlation_id: string - (optional) unique id that identifies distributed transaction
- quote_id: string - unique quote id

**Response body:**
Quote object, null if object wasn't found or error 

### <a name="operation4"></a> Method: 'POST', route '/quotes/create_quote'

Creates a new quote

**Request body:**
- correlation_id: string - (optional) unique id that identifies distributed transaction
- quote: QuoteV1 - Quote object to be created. If object id is not defined it is assigned automatically.

**Response body:**
Created Quote object or error

### <a name="operation5"></a> Method: 'POST', route '/quotes/update_quote'

Updates quote specified by its unique id

**Request body:** 
- correlation_id: string - (optional) unique id that identifies distributed transaction
- quote: QuoteV1 - Quote object with new values. Partial updates are supported

**Response body:**
Updated Quote object or error 
 
### <a name="operation6"></a> Method: 'POST', route '/quotes/delete\_quote\_by_id'

Deletes quote specified by its unique id

**Request body:** 
- correlation_id: string - (optional) unique id that identifies distributed transaction
- quote_id: string - unique quote id

**Response body:**
Occured error or null for success
 
