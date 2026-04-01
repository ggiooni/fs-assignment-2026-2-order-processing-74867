import axios from 'axios';

const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL || 'http://localhost:5145',
});

export interface OrderItem {
  productId: number;
  productName: string;
  quantity: number;
  unitPrice: number;
}

export interface PaymentRecord {
  amount: number;
  status: string;
  transactionId: string | null;
  processedAt: string;
}

export interface ShipmentRecord {
  trackingNumber: string | null;
  carrier: string;
  status: string;
  shippedAt: string | null;
  estimatedDelivery: string | null;
}

export interface Order {
  id: number;
  customerId: number;
  customerName: string;
  orderDate: string;
  status: string;
  totalAmount: number;
  failureReason: string | null;
  correlationId: string;
  items: OrderItem[];
  paymentRecord: PaymentRecord | null;
  shipmentRecord: ShipmentRecord | null;
}

export interface DashboardSummary {
  totalOrders: number;
  pendingOrders: number;
  completedOrders: number;
  failedOrders: number;
  totalRevenue: number;
}

export const getOrders = (status?: string) =>
  api.get<Order[]>('/api/orders', { params: status ? { status } : {} }).then(r => r.data);

export const getOrderById = (id: number) =>
  api.get<Order>(`/api/orders/${id}`).then(r => r.data);

export const getDashboardSummary = () =>
  api.get<DashboardSummary>('/api/orders/dashboard').then(r => r.data);
