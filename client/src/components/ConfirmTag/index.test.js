import React from 'react';
import Enzyme, { shallow } from 'enzyme';

import ConfirmTag from './index';

describe('it renders correctly', () => {
    test('active ConfirmTag renders correctly', () => {
        const component = shallow(<ConfirmTag />);

        expect(component).toMatchSnapshot();
    });

    test('inactive ConfirmTag renders correctly', () => {
        const component = shallow(<ConfirmTag active={false} />);

        expect(component).toMatchSnapshot();
    });
});
