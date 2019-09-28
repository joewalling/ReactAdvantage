import React from 'react';
import BoxShadowContainer from 'components/BoxShadowContainer';
import PageHeader from 'components/PageHeader';
import SearchQuery from 'components/SearchQuery';
import PagedTable from '../PagedTable';

const ListView = ({
  pageHeader,
  filterFields,
  onFilterChange,
  onSearch,
  tableRef,
  ...props
}) => {
  return (
    <BoxShadowContainer>
      <PageHeader
        title={pageHeader.title}
        subtitle={pageHeader.subtitle}
        actions={pageHeader.actions}
      />
      <SearchQuery
        fields={filterFields}
        onChange={onFilterChange}
        onSearch={onSearch}
      />
      <PagedTable {...props} tableRef={tableRef}></PagedTable>
    </BoxShadowContainer>
  );
};

export default ListView;
