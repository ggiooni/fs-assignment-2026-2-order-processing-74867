import { useEffect, useState } from 'react';
import { getOrders } from '../api/orderApi';
import type { Order } from '../api/orderApi';
import { Link } from 'react-router-dom';

const STATUS_COLORS: Record<string, string> = {
  Completed: '#2ecc71',
  Failed: '#e74c3c',
  InventoryFailed: '#e74c3c',
  PaymentFailed: '#e74c3c',
  Submitted: '#3498db',
  InventoryPending: '#f39c12',
  InventoryConfirmed: '#27ae60',
  PaymentPending: '#f39c12',
  PaymentApproved: '#27ae60',
  ShippingPending: '#f39c12',
  ShippingCreated: '#2ecc71',
};

export default function OrdersTable() {
  const [orders, setOrders] = useState<Order[]>([]);
  const [filter, setFilter] = useState('');

  useEffect(() => {
    getOrders().then(setOrders);
    const interval = setInterval(() => getOrders().then(setOrders), 5000);
    return () => clearInterval(interval);
  }, []);

  const filtered = filter ? orders.filter(o => o.status === filter) : orders;
  const statuses = [...new Set(orders.map(o => o.status))].sort();

  return (
    <div>
      <h2>Orders</h2>
      <div style={{ marginBottom: '1rem', display: 'flex', gap: '0.5rem', flexWrap: 'wrap' }}>
        <button onClick={() => setFilter('')} style={{ padding: '0.3rem 0.8rem', background: !filter ? '#3498db' : '#ddd', color: !filter ? 'white' : 'black', border: 'none', borderRadius: '4px', cursor: 'pointer' }}>All ({orders.length})</button>
        {statuses.map(s => (
          <button key={s} onClick={() => setFilter(s)} style={{ padding: '0.3rem 0.8rem', background: filter === s ? (STATUS_COLORS[s] || '#999') : '#ddd', color: filter === s ? 'white' : 'black', border: 'none', borderRadius: '4px', cursor: 'pointer' }}>
            {s} ({orders.filter(o => o.status === s).length})
          </button>
        ))}
      </div>
      <table style={{ width: '100%', borderCollapse: 'collapse' }}>
        <thead>
          <tr style={{ borderBottom: '2px solid #ddd', textAlign: 'left' }}>
            <th style={{ padding: '0.75rem' }}>ID</th>
            <th style={{ padding: '0.75rem' }}>Customer</th>
            <th style={{ padding: '0.75rem' }}>Date</th>
            <th style={{ padding: '0.75rem' }}>Total</th>
            <th style={{ padding: '0.75rem' }}>Status</th>
            <th style={{ padding: '0.75rem' }}></th>
          </tr>
        </thead>
        <tbody>
          {filtered.map(order => (
            <tr key={order.id} style={{ borderBottom: '1px solid #eee' }}>
              <td style={{ padding: '0.75rem' }}>#{order.id}</td>
              <td style={{ padding: '0.75rem' }}>{order.customerName}</td>
              <td style={{ padding: '0.75rem' }}>{new Date(order.orderDate).toLocaleString()}</td>
              <td style={{ padding: '0.75rem' }}>${order.totalAmount.toFixed(2)}</td>
              <td style={{ padding: '0.75rem' }}>
                <span style={{ background: STATUS_COLORS[order.status] || '#999', color: 'white', padding: '0.2rem 0.6rem', borderRadius: '12px', fontSize: '0.85rem' }}>{order.status}</span>
              </td>
              <td style={{ padding: '0.75rem' }}>
                <Link to={`/orders/${order.id}`} style={{ color: '#3498db' }}>Details</Link>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
      {filtered.length === 0 && <p style={{ textAlign: 'center', color: '#999', padding: '2rem' }}>No orders found.</p>}
    </div>
  );
}
