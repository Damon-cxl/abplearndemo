const ENV = {
  dev: {
    apiUrl: 'http://localhost:44307',
    oAuthConfig: {
      issuer: 'http://localhost:44307',
      clientId: 'Books_App',
      clientSecret: '1q2w3e*',
      scope: 'Books',
    },
    localization: {
      defaultResourceName: 'Books',
    },
  },
  prod: {
    apiUrl: 'http://localhost:44307',
    oAuthConfig: {
      issuer: 'http://localhost:44307',
      clientId: 'Books_App',
      clientSecret: '1q2w3e*',
      scope: 'Books',
    },
    localization: {
      defaultResourceName: 'Books',
    },
  },
};

export const getEnvVars = () => {
  // eslint-disable-next-line no-undef
  return __DEV__ ? ENV.dev : ENV.prod;
};
