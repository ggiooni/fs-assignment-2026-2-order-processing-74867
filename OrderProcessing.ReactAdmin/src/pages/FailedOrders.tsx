import { useEffect, useState } from 'react';
import { getOrders } from '../api/orderApi';
import type { Order } from '../api/orderApi';
import { Link } from 'react-router-dom';

export default function FailedOrders() {
  const [orders, setOrders] = useState<Order[]>([]);

  useEffect(() => {
    getOrders().then(all => setOrders(all.filter(o =>
      ['Failed', 'InventoryFailed', 'PaymentFailed'].includes(o.status)
    )));
  }, []);

  return (
    <div>
      <Link to="/orders" style={{ color: '#3498db' }}>&larr; Back to Orders</Link>
      <h2 style={{ marginTop: '0.5rem', color: '#e74c3c' }}>Failed Orders</h2>

      {orders.length === 0 ? (
        <p style={{ color: '#999' }}>No failed orders found.</p>
      ) : (
        <table style={{ width: '100%', borderCollapse: 'collapse' }}>
          <thead>
            <tr style={{ borderBottom: '2px solid #ddd', textAlign: 'left' }}>
              <th style={{ padding: '0.75rem' }}>ID</th>
              <th style={{ padding: '0.75rem' }}>Customer</th>
              <th style={{ padding: '0.75rem' }}>Status</th>
              <th style={{ padding: '0.75rem' }}>Failure Reason</th>
              <th style={{ padding: '0.75rem' }}>Total</th>
              <th style={{ padding: '0.75rem' }}></th>
            </tr>
          </thead>
          <tbody>
            {orders.map(order => (
              <tr key={order.id} style={{ borderBottom: '1px solid #eee' }}>
                <td style={{ padding: '0.75rem' }}>#{order.id}</td>
                <td style={{ padding: '0.75rem' }}>{order.customerName}</td>
                <td style={{ padding: '0.75rem' }}>
                  <span style={{ background: '#e74c3c', color: 'white', padding: '0.2rem 0.6rem', borderRadius: '12px', fontSize: '0.85rem' }}>{order.status}</span>
                </td>
                <td style={{ padding: '0.75rem' }}>{order.failureReason || 'Unknown'}</td>
                <td style={{ padding: '0.75rem' }}>${order.totalAmount.toFixed(2)}</td>
                <td style={{ padding: '0.75rem' }}>
                  <Link to={`/orders/${order.id}`} style={{ color: '#3498db' }}>Details</Link>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}
