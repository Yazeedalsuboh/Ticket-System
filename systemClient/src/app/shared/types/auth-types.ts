export interface register {
  fullName: string;
  email: string;
  password: string;
  mobile: string;
  dateOfBirth: string;
  address: string;
  image: string;
}

export interface addTicket {
  title: string;
  description: string;
  productName: string;
  attachments?: File[];
  priority: string;
}

export interface productName {
  name: string;
}
export interface login {
  identifier: string;
  password: string;
}
export interface Token {
  token: string;
  refreshToken: string;
}

export interface TokenResponse {
  token: string;
  refreshToken: string;
}

export interface Profile {
  role: string;
  name: string;
  email: string;
}
