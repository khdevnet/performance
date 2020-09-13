# postman
[newman cmd runner](https://www.npmjs.com/package/newman)
[run in parallel](https://medium.com/@mnu/run-multiple-postman-collection-in-parallel-stress-ee20801922ed)
[arg parse](https://www.npmjs.com/package/argparse)
### Create nodejs runner
```cmd
npm init -y
npm i newman path argparse concurrently
```

#### Run using runner multiple instances
```cmd
node .\runner.js -c "https://www.getpostman.com/collections/33333344444444444" -e 'environments/dev_env.postman_environment' -n 10 -p 5 --silent false
```
#### run using newman
```cmd
newman run https://www.getpostman.com/collections/33333344444444444 -e dev_env.postman_environment.json -k -n 10
```

#### run in parallel
* create config file 'runners.config.json'
```json
[
    {
        "command": "node runner.js",
        "args": [
            {
                "key": "-c",
                "value": "https://api.getpostman.com/collections/{uid}?apikey={key}"
            },
            {
                "key": "-e",
                "value": "https://api.getpostman.com/environments/{uid}?apikey={key}"
            },
            {
                "key": "-n",
                "value": 10000
            },
            {
                "key": "--silent",
                "value": false
            },
            {
                "key": "--reporters",
                "value": "json"
            }
        ]
    },
    {
        "command": "node runner.js",
        "args": [
            {
                "key": "-c",
                "value": "https://api.getpostman.com/collections/{uid}?apikey={key}"
            },
            {
                "key": "-e",
                "value": "https://api.getpostman.com/environments/{uid}?apikey={key}"
            },
            {
                "key": "-n",
                "value": 10000
            },
            {
                "key": "--silent",
                "value": false
            },
            {
                "key": "--reporters",
                "value": "json"
            }
        ]
    }
]
```
* run runners
```cmd
node runners.js
```