import { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import { getOrderById } from '../api/orderApi';
import type { Order } from '../api/orderApi';

export default function OrderDetails() {
  const { id } = useParams<{ id: string }>();
  const [order, setOrder] = useState<Order | null>(null);

  useEffect(() => {
    if (id) {
      getOrderById(parseInt(id)).then(setOrder);
      const interval = setInterval(() => getOrderById(parseInt(id)).then(setOrder), 3000);
      return () => clearInterval(interval);
    }
  }, [id]);

  if (!order) return <p>Loading...</p>;

  return (
    <div>
      <Link to="/orders" style={{ color: '#3498db' }}>&larr; Back to Orders</Link>
      <h2 style={{ marginTop: '0.5rem' }}>Order #{order.id}</h2>

      <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '1rem', marginTop: '1rem' }}>
        <Section title="Order Info">
          <p><strong>Customer:</strong> {order.customerName} (ID: {order.customerId})</p>
          <p><strong>Date:</strong> {new Date(order.orderDate).toLocaleString()}</p>
          <p><strong>Status:</strong> {order.status}</p>
          <p><strong>Total:</strong> ${order.totalAmount.toFixed(2)}</p>
          <p><strong>Correlation ID:</strong> <code>{order.correlationId}</code></p>
          {order.failureReason && <p style={{ color: '#e74c3c' }}><strong>Failure:</strong> {order.failureReason}</p>}
        </Section>

        <Section title="Items">
          <table style={{ width: '100%' }}>
            <thead><tr><th>Product</th><th>Qty</th><th>Price</th></tr></thead>
            <tbody>
              {order.items.map((item, i) => (
                <tr key={i}><td>{item.productName}</td><td>{item.quantity}</td><td>${item.unitPrice.toFixed(2)}</td></tr>
              ))}
            </tbody>
          </table>
        </Section>

        {order.paymentRecord && (
          <Section title="Payment">
            <p><strong>Status:</strong> {order.paymentRecord.status}</p>
            <p><strong>Amount:</strong> ${order.paymentRecord.amount.toFixed(2)}</p>
            <p><strong>Transaction:</strong> {order.paymentRecord.transactionId || 'N/A'}</p>
            <p><strong>Processed:</strong> {new Date(order.paymentRecord.processedAt).toLocaleString()}</p>
          </Section>
        )}

        {order.shipmentRecord && (
          <Section title="Shipment">
            <p><strong>Tracking:</strong> {order.shipmentRecord.trackingNumber}</p>
            <p><strong>Carrier:</strong> {order.shipmentRecord.carrier}</p>
            <p><strong>Status:</strong> {order.shipmentRecord.status}</p>
            {order.shipmentRecord.shippedAt && <p><strong>Shipped:</strong> {new Date(order.shipmentRecord.shippedAt).toLocaleString()}</p>}
            {order.shipmentRecord.estimatedDelivery && <p><strong>Est. Delivery:</strong> {new Date(order.shipmentRecord.estimatedDelivery).toLocaleDateString()}</p>}
          </Section>
        )}
      </div>
    </div>
  );
}

function Section({ title, children }: { title: string; children: React.ReactNode }) {
  return (
    <div style={{ background: 'white', border: '1px solid #ddd', borderRadius: '8px', padding: '1rem' }}>
      <h4 style={{ marginTop: 0 }}>{title}</h4>
      {children}
    </div>
  );
}
