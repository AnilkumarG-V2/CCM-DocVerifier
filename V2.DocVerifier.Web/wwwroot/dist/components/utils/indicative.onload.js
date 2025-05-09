var LoanAppAnalytics;
(function () {
    try {
        LoanAppAnalytics = _analytics.init({
            app: 'analytics-Random',
            debug: environmentName.toLowerCase() != "production" ? true : false,
            plugins: [
                analyticsIndicative({
                    apiKey: apiKeyValue,
                })
            ]
        });
    } catch (ex) {
    }
})();
