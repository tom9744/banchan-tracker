export interface Banchan {
  id: string;
  name: string;
  createdAt: string;
}

export interface CreateBanchanDto {
  name: string;
}

export interface UpdateBanchanDto {
  name: string;
}
