import React from 'react';
import { Menubar as PrimeMenubar } from 'primereact/menubar';
import { withRouter } from 'react-router-dom';

function Menubar({ history }) {
  const items = [
    {
      label: 'Dashboard',
      command: () => history.push('/'),
    },
    {
      label: 'Users',
      command: () => history.push('/users'),
    },
    {
      label: 'Roles',
      command: () => history.push('/roles'),
    },
    {
      label: "Tenants",
      command: () => history.push('/tenants')
    }
  ];

  return <PrimeMenubar model={items} />;
}

export default withRouter(Menubar);
