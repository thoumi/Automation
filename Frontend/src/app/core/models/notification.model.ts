export interface NotificationRecipient {
  id: number;
  type: NotificationType;
  identifier: string;
  name: string;
  isActive: boolean;
  createdAt: Date;
}

export enum NotificationType {
  Email = 0,
  WhatsApp = 1,
  Chime = 2
}

