const { ArgumentParser } = require('argparse')
const path = require('path')
const async = require('async')
const newman = require('newman');

const parser = new ArgumentParser({
    description: 'Options'
});

parser.add_argument('-c', '--collection', { help: 'url to collection' });
parser.add_argument('-e', '--environment', { help: 'url to environment' });
parser.add_argument('-k', '--insecure', { help: 'Skip SSL verification', default: true });
parser.add_argument('-n', '--iterationCount', { help: 'iteration count', default: '1' });
parser.add_argument('--silent', { help: 'Disable terminal output', default: 'false' });
parser.add_argument('--reporters', { help: 'Disable terminal output', default: 'cli' });
parser.add_argument('-f','--folder', { help: 'Set folder' });
var args = parser.parse_args();

const parametersForTestRun = {
    collection: args.collection,
    environment: require(args.environment),
    reporters: args.reporters,
    insecure: args.insecure,
    iterationCount: parseInt(args.iterationCount),
    silent: args.silent.toLowerCase() === 'true',
    folder: args.folder
};

newman.run(parametersForTestRun,
    (err, summary) => {
        err && console.error(err);
        var failures = summary.run.failures;
        var status = failures.length
            ? JSON.stringify(failures.failures, null, 2)
            : `${summary.collection.name} ran successfully.`;
        console.info(status);
    });

