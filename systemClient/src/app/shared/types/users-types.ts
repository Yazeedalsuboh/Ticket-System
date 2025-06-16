export interface Employee {
  id: string;
  fullName: string;
}

export interface User {
  id: string;
  fullName: string;
  mobile: string;
  email: string;
  role: number;
  ticketCount: number;
  isActive: boolean;
}

export enum UserRoles {
  Manager = 'Manager',
  Client = 'Client',
  Support = 'Support',
}
