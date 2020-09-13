const concurrently = require('concurrently');
const { parse } = require('path');
const fs = require('fs');
const { ArgumentParser } = require('argparse')
const path = require('path')

const parser = new ArgumentParser({
    description: 'Options'
});

parser.add_argument('--runners', { help: 'path to runners config', default: 'runners.config.json' });
parser.add_argument('-n', '--instances', { help: 'number of instances', default: '1' });

var args = parser.parse_args();

var configPath = path.join(__dirname, args.runners)
var runners = JSON.parse(fs.readFileSync(configPath));

function createCommand(commandOptions) {
    var args = '';
    commandOptions.args.forEach(arg => {
        args += ` ${arg.key} "${arg.value}"`
    });

    return `${commandOptions.command} ${args}`;
}

var logStream = fs.createWriteStream('logs.txt');
var commands = [];
for (let index = 0; index < parseInt(args.instances); index++) {
    console.log(runners.map(createCommand));
    commands = [...commands,...runners.map(createCommand)]
}

concurrently(commands, {
    prefix: 'name',
    killOthers: ['failure', 'success'],
    restartTries: 3,
    outputStream: logStream
}).then((data) => { console.log(data); }, (d) => { console.log(d); });
