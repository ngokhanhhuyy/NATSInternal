class HttpClient {
  private cookie: string | null = null;
  private host: string = "http://localhost:5000/api"

  public async authenticateAsync() {
    const response = await fetch(this.host + "/authentication/get-access-cookie", {
      method: "post",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        userName: "ngokhanhhuyy",
        password: "Huyy47b1"
      })
    });

    if (response.status !== 200) {
      throw new Error(`Authentication failed with status code: ${response.status}`);
    }

    this.cookie = response.headers.get("Set-Cookie");
  }

  public async postAsync<TResponseDto>(endpoint: string, requestDto: object): Promise<TResponseDto> {
    return await this.sendAsync<TResponseDto>(endpoint, "post", requestDto);
  }
  
  public async postAndIgnoreAsync(endpoint: string, requestDto: object): Promise<void> {
    return await this.sendAsync(endpoint, "post", requestDto, true);
  }

  public async sendAsync(endpoint: string, method: string, body: object | null, ignoreResponse: true): Promise<void>;
  public async sendAsync<TResponseDto>(endpoint: string, method: string, body: object | null): Promise<TResponseDto>;
  public async sendAsync<TResponseDto>(endpoint: string, method: string, body: object | null, ignoreResponse: false): Promise<TResponseDto>;
  public async sendAsync<TResponseDto>(endpoint: string, method: string, body: object | null, ignoreResponse: boolean = false): Promise<TResponseDto | void> {
    const response = await fetch(this.host + endpoint, {
      method,
      headers: {
        "Content-Type": "application/json",
        "Cookie": this.cookie!
      },
      body: body !== null && method.toLowerCase() !== "get" ? JSON.stringify(body) : undefined
    });

    if (!ignoreResponse) {
      return await response.json() as TResponseDto;
    }
  }
}

type ProductListRequestDto = Partial<{
  sortByAscending: boolean;
  sortByFieldName: string;
  page: number;
  resultsPerPage: number;
}>;

type Product

async function run() {
  const httpClient = new HttpClient();
  await httpClient.authenticateAsync();
}

run();
