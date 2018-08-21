import React from 'react';
import Enzyme, { shallow } from 'enzyme';

import SearchQuery from './index';

it('renders correctly', () => {
    const fields = [{
        name: 'test',
        operators: 'all',
        label: 'test',
        input: {
            type: 'text'
        }
    }];

    const component = shallow(
        <SearchQuery fields={fields} />
    );

    expect(component).toMatchSnapshot();
});