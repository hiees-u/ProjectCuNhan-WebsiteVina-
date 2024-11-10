export interface Notification {
  messages: string;
  status: 'error' | 'warning' | 'info' | 'success'; // Bạn có thể thêm các giá trị khác nếu cần
}

export function ConstructerNotification():Notification {
  return {
    messages: '',
    status: 'info'
  }
}
