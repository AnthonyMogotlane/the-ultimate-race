using UltimateRace.Common;

var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins"; // Policy name is arbitrary

// Register CORS policy to allow requests from the frontend (e.g. Angular dev server)
builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins, policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins);

var scoreboard = new ScoreBoard();
var vehicle = new Vehicle();
var random = new Random();
var raceStarted = false;
var raceResults = new List<string>();

var contestantStatus = new Dictionary<string, bool>
{
    ["chopper"] = false,
    ["bike"] = false,
    ["tesla"] = false,
    ["nuclearsub"] = false
};

// Race endpoints
app.MapGet("/", () =>
{
    var html = """
    <html>
    <head>
        <title>Ultimate Race API</title>
        <style>
            body { font-family: Arial, sans-serif; margin: 40px; }
            .container { max-width: 800px; margin: 0 auto; }
            .endpoint { background: #f5f5f5; padding: 15px; margin: 10px 0; border-radius: 5px; }
            code { background: #e0e0e0; padding: 2px 5px; border-radius: 3px; }
        </style>
    </head>
    <body>
        <div class="container">
            <h1>The Ultimate Race API</h1>
            <p>Welcome to the Ultimate Race from Cairo to Cape Town!</p>
            
            <div class="endpoint">
                <h3>Start the race:</h3>
                <p><code>POST /race/start</code></p>
            </div>
            
            <div class="endpoint">
                <h3>Check race status:</h3>
                <p><code>GET /race/status</code></p>
            </div>
            
            <div class="endpoint">
                <h3>Get positions:</h3>
                <p><code>GET /race/positions</code></p>
            </div>
            
            <div class="endpoint">
                <h3>Get results:</h3>
                <p><code>GET /race/results</code></p>
            </div>
        </div>
    </body>
    </html>
    """;

    return Results.Content(html, "text/html");
});

app.MapPost("/race/start", async (HttpContext context) =>
{
    if (raceStarted)
    {
        return Results.BadRequest("Race already started!");
    }

    raceStarted = true;
    raceResults.Clear();

    // Start all vehicles in parallel
    var tasks = new[]
    {
        Task.Run(() => RunChopper()),
        Task.Run(() => RunBike()),
        Task.Run(() => RunTesla()),
        Task.Run(() => RunNuclearSub()),
        Task.Run(() => MonitorRace())
    };

    return Results.Ok(new
    {
        message = "Race started! Check /race/status for updates.",
        timestamp = DateTime.UtcNow
    });
});

app.MapGet("/race/status", () =>
{
    var status = new
    {
        raceStarted,
        contestantsFinished = contestantStatus.Count(c => c.Value),
        contestantsRemaining = contestantStatus.Count(c => !c.Value),
        positions = new
        {
            chopper = scoreboard.ChopperPosition,
            bike = scoreboard.BikePosition,
            tesla = scoreboard.TeslaPosition,
            nuclearsub = scoreboard.SubPosition
        },
        distancesToWin = new
        {
            chopper = scoreboard.ChopperDistanceToTravelToWin,
            bike = scoreboard.BikeDistanceToTravelToWin,
            tesla = scoreboard.TeslaDistanceToTravelToWin,
            nuclearsub = scoreboard.SubDistanceToTravelToWin
        },
        lastUpdated = DateTime.UtcNow
    };

    return Results.Ok(status);
});

app.MapGet("/race/positions", () =>
{
    var positions = new[]
    {
        new { Vehicle = "Chopper", Position = scoreboard.ChopperPosition },
        new { Vehicle = "Bike", Position = scoreboard.BikePosition },
        new { Vehicle = "Tesla", Position = scoreboard.TeslaPosition },
        new { Vehicle = "Nuclear Sub", Position = scoreboard.SubPosition }
    }.OrderByDescending(p => p.Position);

    return Results.Ok(positions);
});

app.MapGet("/race/results", () =>
{
    return Results.Ok(raceResults);
});

// Vehicle methods
async Task RunChopper()
{
    int fuel = vehicle.ChopperFuelCapacityGallons;

    while (!contestantStatus["chopper"])
    {
        await Task.Delay(1000); // 1 second = 1 hour
        scoreboard.ChopperPosition += vehicle.ChopperAvgSpeedKmh;
        fuel -= vehicle.ChopperFuelUsagePerHourGallons;

        if (fuel < 0)
        {
            await Task.Delay(vehicle.ChopperTimeToRefuelHrs * 1000);
            fuel = vehicle.ChopperFuelCapacityGallons;
        }

        if (random.NextDouble() < vehicle.ChopperBreakdownProbability)
        {
            int repairTime = (int)(random.NextDouble() / random.NextDouble() * 1000) + 1000;
            await Task.Delay(repairTime);
        }
    }
}

async Task RunBike()
{
    int fuel = (int)vehicle.BikeFuelTankLiters;

    while (!contestantStatus["bike"])
    {
        await Task.Delay(1000);
        scoreboard.BikePosition += (int)(vehicle.BikeSpeedMph * 1.6);
        fuel -= (int)(vehicle.BikeSpeedMph / vehicle.BikeKmPerLitre);

        if (fuel < 0)
        {
            await Task.Delay((int)(vehicle.BikeTimeToRefuelHrs * 1000));
            fuel = (int)vehicle.BikeFuelTankLiters;
        }

        if (random.NextDouble() < vehicle.BikeBreakdownProbability)
        {
            int repairTime = (int)(random.NextDouble() / random.NextDouble() * 1000) + 1000;
            await Task.Delay(repairTime);
        }
    }
}

async Task RunTesla()
{
    double batteryLeft = vehicle.TeslaBatteryPack;

    while (!contestantStatus["tesla"])
    {
        await Task.Delay(1000);
        scoreboard.TeslaPosition += vehicle.TeslaSpeed;
        batteryLeft -= vehicle.TeslaEngineKw;

        if (batteryLeft < 0)
        {
            await Task.Delay((int)vehicle.TeslaTimeToRefuelHrs * 1000);
            batteryLeft = vehicle.TeslaBatteryPack;
        }

        if (random.NextDouble() < vehicle.TeslaBreakdownProbability)
        {
            int repairTime = (int)(random.NextDouble() / random.NextDouble() * 1000) + 1000;
            await Task.Delay(repairTime);
        }
    }
}

async Task RunNuclearSub()
{
    while (!contestantStatus["nuclearsub"])
    {
        await Task.Delay(1000);
        scoreboard.SubPosition += vehicle.NuclearSubSpeedKnots * 2;

        if (random.NextDouble() < vehicle.SubBreakdownProbability)
        {
            int repairTime = (int)(random.NextDouble() / random.NextDouble() * 1000) + 1000;
            await Task.Delay(repairTime);
        }
    }
}

// Race monitoring
async Task MonitorRace()
{
    int position = 0;

    while (contestantStatus.Any(c => !c.Value))
    {
        await Task.Delay(1000);

        // Check for winners
        if (!contestantStatus["bike"] && scoreboard.BikePosition > scoreboard.BikeDistanceToTravelToWin)
        {
            contestantStatus["bike"] = true;
            position++;
            var result = position == 1
                ? "?? Bike has won the race!"
                : $"Bike came {position}nd.";
            raceResults.Add(result);
        }

        if (!contestantStatus["tesla"] && scoreboard.TeslaPosition > scoreboard.TeslaDistanceToTravelToWin)
        {
            contestantStatus["tesla"] = true;
            position++;
            var result = position == 1
                ? "?? Tesla has won the race!"
                : $"Tesla came {position}nd.";
            raceResults.Add(result);
        }

        if (!contestantStatus["chopper"] && scoreboard.ChopperPosition > scoreboard.ChopperDistanceToTravelToWin)
        {
            contestantStatus["chopper"] = true;
            position++;
            var result = position == 1
                ? "?? Chopper has won the race!"
                : $"Chopper came {position}nd.";
            raceResults.Add(result);
        }

        if (!contestantStatus["nuclearsub"] && scoreboard.SubPosition > scoreboard.SubDistanceToTravelToWin)
        {
            contestantStatus["nuclearsub"] = true;
            position++;
            var result = position == 1
                ? "?? Nuclear Sub has won the race!"
                : $"Nuclear Sub came {position}nd.";
            raceResults.Add(result);
        }
    }

    raceResults.Add("?? Race completed! All vehicles have finished.");
}

app.Run();