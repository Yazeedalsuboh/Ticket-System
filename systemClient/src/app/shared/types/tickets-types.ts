export interface ticketBrief {
  id: string;
  title: string;
  status: TicketStatus;
  clientName: string;
  supportName: string;
  productName: string;
}

export interface Attachment {
  fileName: string;
  fileUrl: string;
}

export interface Comment {
  message: string;
  createdAt: Date;
  username: string;
}

export interface Ticket {
  title: string;
  description: string;
  status: TicketStatus;
  clientName: string;
  supportName: string;
  productName: string;
  attachments: Attachment[];
  comments: Comment[];
  priority: string;
}

export interface AddComment {
  id: string;
  message: string;
}

export enum TicketStatus {
  All = 'All',
  Open = 'Open',
  Closed = 'Closed',
  Assign = 'Assign',
}

export interface AssignTicket {
  tickedId: string;
  employeeId: string;
}
