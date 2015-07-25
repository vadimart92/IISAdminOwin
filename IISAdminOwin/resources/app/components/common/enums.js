define([], function() {
  return {
    PoolState: {
      Running: {
        value: 0,
        displayValue: "Running"
      },
      Restarting: {
        value: 1,
        displayValue: "Restarting",
        Stopped: {
          value: 2,
          displayValue: "Stopped"
        }
      }
    },
    SiteState: {
      Started: {
        value: "Started",
        displayValue: "Running"
      },
      Restarting: {
        value: 1,
        displayValue: "Restarting",
        Stopped: {
          value: 2,
          displayValue: "Stopped"
        }
      }
    }
  };
});
