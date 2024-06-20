
`# RedditLiveStats

## Project Structure

- **RedditLiveStats**: This is the main backend project.
- **RedditLiveStatsUI**: This is the frontend React application.
- **RedditLiveStats.Tests**: This is the test project for unit and integration tests.

## Prerequisites

- .NET 7 SDK
- Node.js
- npm (Node Package Manager)

## Setup and Configuration

### Backend (RedditLiveStats)

1. **Clone the repository**:
   ```bash
   git clone https://github.com/yourusername/RedditLiveStats.git
   cd RedditLiveStats
   ```

2.  **Add your Reddit API credentials**: Update the `appsettings.json` file with your Reddit API AccessToken.
    
    `{
      "Reddit": {
        "AccessToken": "your_reddit_access_token"
      },
      "Logging": {
        "LogLevel": {
          "Default": "Information",
          "Microsoft": "Warning",
          "Microsoft.Hosting.Lifetime": "Information"
        }
      },
      "AllowedHosts": "*"
    }` 
    
3.  **Restore dependencies and run the project**:
    ```bash
    dotnet restore
    dotnet build
    dotnet run
    ```
    
### Frontend (RedditLiveStatsUI)

1.  **Navigate to the React project directory**:
    
     ```bash
    cd RedditLiveStats/RedditLiveStatsUI/reddit-live-stats-client
    ```
    
2.  **Install dependencies**:
   
    ```bash
    npm install
    ```
    
3.  **Start the React development server**:
    
    ```bash
    `npm start`
    

## Running Tests

1.  **Navigate to the test project directory**:
    
    ```bash
    cd RedditLiveStats.Tests
    ```
    
2.  **Run the tests**:

    ```bash
    dotnet test
    ```
    

## Explanation of Fetching Logic

The application fetches the latest 10,000 posts from the "wallstreetbets" subreddit and aggregates statistics for the top posts and users over the past month.

### Backend Services

#### `RedditService`

This service handles interaction with the Reddit API to fetch posts.

-   **URL**: `https://oauth.reddit.com/r/wallstreetbets/new`
-   **Headers**:
    -   `Authorization`: Bearer token
    -   `User-Agent`: Custom user agent
    -   `Accept`: `*/*`
    -   `Accept-Encoding`: `gzip, deflate, br`
    -   `Connection`: `keep-alive`

#### `StatisticsService`

This service processes fetched posts and aggregates statistics for top posts and users.

-   **FetchAndProcessPostsAsync**: Fetches posts and processes them.
-   **ProcessPost**: Adds or updates post statistics.
-   **GetTopPosts**: Retrieves top posts within the last month.
-   **GetTopUsers**: Retrieves top users aggregated by post count and upvotes over the last month.

### Background Service

#### `RedditBackgroundService`

A hosted service that periodically fetches and processes posts.

-   **Interval**: Every 1 minute

### Frontend

The frontend React application fetches and displays top posts and users, updating every 5 seconds.

### Gitignore

To prevent unnecessary files from being tracked by Git, the following entries should be added to `.gitignore`:

`node_modules/
RedditLiveStats.Tests/bin/
RedditLiveStats.Tests/obj/
RedditLiveStats/bin/
RedditLiveStats/obj/` 

## React Startup Process

1.  **Navigate to the React project directory**: 
    ```bash
    cd RedditLiveStats/RedditLiveStatsUI/reddit-live-stats-client
    ``` 
    
2.  **Install dependencies**:
    ```bash
    npm install
    ```
    
3.  **Start the development server**:
    ```bash
    `npm start` 
    ```
    

This will start the React development server and open the application in your default browser.

## Note

-   Ensure you have the Reddit API AccessToken configured in `appsettings.json` before running the backend service.
-   The React application assumes the backend service is running on `https://localhost:7295`. Update the API URL in the React application if necessary.

