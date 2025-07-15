import type {
  Banchan,
  CreateBanchanDto,
  UpdateBanchanDto,
} from "../types/banchan";

const API_BASE_URL = "http://localhost:5121/api";

class ApiService {
  private async request<T>(
    endpoint: string,
    options: RequestInit = {}
  ): Promise<T> {
    const url = `${API_BASE_URL}${endpoint}`;
    const response = await fetch(url, {
      headers: {
        "Content-Type": "application/json",
        ...options.headers,
      },
      ...options,
    });

    if (!response.ok) {
      const errorData = await response.json().catch(() => ({}));
      throw new Error(
        errorData.message || `HTTP error! status: ${response.status}`
      );
    }

    // DELETE 요청의 경우 JSON 응답이 없으므로 파싱하지 않음
    if (options.method === "DELETE") {
      return {} as T;
    }

    return response.json();
  }

  async getBanchans(): Promise<Banchan[]> {
    return this.request<Banchan[]>("/banchan");
  }

  async getBanchanById(id: string): Promise<Banchan> {
    return this.request<Banchan>(`/banchan/${id}`);
  }

  async createBanchan(data: CreateBanchanDto): Promise<Banchan> {
    return this.request<Banchan>("/banchan", {
      method: "POST",
      body: JSON.stringify(data),
    });
  }

  async updateBanchan(id: string, data: UpdateBanchanDto): Promise<Banchan> {
    return this.request<Banchan>(`/banchan/${id}`, {
      method: "PUT",
      body: JSON.stringify(data),
    });
  }

  async deleteBanchan(id: string): Promise<void> {
    return this.request<void>(`/banchan/${id}`, {
      method: "DELETE",
    });
  }
}

export const apiService = new ApiService();
