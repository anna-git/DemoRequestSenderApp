# How to run


docker build -t requestsender ../ -f ./Dockerfile

docker run --rm -p 84:84 -e "ASPNETCORE_URLS=http://+:84" -e ASPNETCORE_ENVIRONMENT=Development -t requestsender

# How to use

For headers, use arrays for values, for example 
```json
{
	"method":"POST",
	"headers":
	{
		"Content-Type":["application/json", "something/else"]
	},
	"body": "body content"
}
```


