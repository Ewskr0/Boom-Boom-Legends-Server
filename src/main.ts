process
    .once('SIGINT', () => process.exit(1))
    .once('SIGTERM', () => process.exit(1));
